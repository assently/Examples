import jwt, { JwtPayload } from "jsonwebtoken"
import uuid4 from "uuid4"

export class AssentlyTokenUserFlow {
    private static secret = process.env.ASSENTLY_SECRET! // secret provided by assently, used to sign request token
    private static identityKey = process.env.ASSENTLY_IDENTITY_KEY! // secret identity key by assently, used to validate response
    private static issuer = process.env.ASSENTLY_ISSUER! // issuer provided by assently usually your company or name
    private static domain_name = process.env.ASSENTLY_DOMAIN_NAME! // your personal identifier, during testing use "Test av Mobilt BankID"
    private static assently_url = process.env.ASSENTLY_COREID_URL! // coreid-test.assently.com or the production base url that assently client script uses

    /**
     * @description Generates a basic token to be sent to Assently.
     * @param origin The base origin url this token is sent from ie localhost:3000 or your production url
     */
    static generateAssentlyAuthToken(origin: string): string {
        const startTime = Math.floor(Date.now())
        const endTime = startTime + 600 // 10 minn

        const authToken = {
            jti: uuid4(),
            iss: this.issuer,
            aud: "all",
            iat: startTime,
            exp: endTime,
            hst: origin,
            dnm: this.domain_name,
        }

        return jwt.sign(authToken, this.secret)
    }

    /**
     * @description This function validates a few things, first it validates it has come from assently and has
     * not been tampered with, secondly it validates all the parameters are correct and the jti identifier matches
     * the outgoing jti sent to assently. If any of this fails to meet the requirements it will fail to validate and return null.
     * If it is successful it will return the token to be used as needed
     * @param token The token received from Assently
     * @param requestToken The token sent to Assently. Required here to validate the auth_jti.
     */
    static validateIdentityToken(token: string, requestToken: string) {
        const decodedRequestToken = jwt.decode(requestToken)
        const verifiedToken = this.verifyToken(token)
        if (!verifiedToken) throw new Error("could not verify JWT has not been tampered with from Assently")
        if (!decodedRequestToken) throw new Error("could not verify JWT has not been tampered with from Assently")

        const isValidParams = this.checkJwtCredentials(verifiedToken, decodedRequestToken as jwt.JwtPayload)
        if (isValidParams) return verifiedToken
        return null
    }

    private static verifyToken(token: string): JwtPayload | null {
        try {
            return jwt.verify(token, this.identityKey) as JwtPayload
        } catch (err: any) {
            console.error("Token validation error:", err)
            throw err
        }
    }

    private static checkJwtCredentials(jwt: jwt.JwtPayload, requestToken: jwt.JwtPayload): boolean {
        return (
            (jwt.iss === this.assently_url && // the url the token is sent from
                jwt.aud === this.issuer && // the issuer id provided by assently
                jwt.exp &&
                jwt.iat && // expiry and issue time exists
                jwt.exp > jwt.iat && // expiry is after issue time
                jwt["auth_jti"] === requestToken.jti) === // the id from the request jwt matches the response jwt
            true
        )
    }
}
