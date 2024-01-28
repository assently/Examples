"use server"

import { AssentlyTokenUserFlow } from "@/server/business/assently/assentlyUserFlow"

export async function getAssentlyJwtTokenAction(originURL: string): Promise<string> {
    return AssentlyTokenUserFlow.generateAssentlyAuthToken(originURL)
}

export async function validateTokenAction(token: string, outGoingToken: string): Promise<boolean> {
    try {
        const tokenValidation = AssentlyTokenUserFlow.validateIdentityToken(token, outGoingToken)
        if (tokenValidation) {
            // use token and user info as needed to create a user and or session

            return true // this could return anything, would just need to update what triggers handle success in the client
        }
        return false
    } catch (e: any) {
        console.log(e)
        throw e
    }
}
