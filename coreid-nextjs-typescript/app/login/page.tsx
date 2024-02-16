"use client"

import { useEffect, useRef, useState } from "react"
import { getAssentlyJwtTokenAction, validateTokenAction } from "@/server/actions/assentlyActions"

declare global {
    interface Window {
        coreid_client: any
    }
}

export default function CoreIDPage() {
    const [token, setToken] = useState<string | null>(null)
    const [successState, setSuccessState] = useState<string>("NOT authentication")
    const ran = useRef(false)

    useEffect(() => {
        if (ran.current) return
        ran.current = true
        const origin = window.location.origin
        getAssentlyJwtTokenAction(origin).then((res) => {
            setToken(res)
        })
    }, [])

    useEffect(() => {
        if (!token) return

        // Dynamically load the CoreID script
        const script = document.createElement("script")
        script.src = process.env.NEXT_PUBLIC_COREID_URL! // This should be either the test or production URL
        script.async = true
        document.body.appendChild(script)

        script.onload = () => {
            if (window.coreid_client) {
                window.coreid_client.init({ // ensure this config matches your requirements based on the Assently documentation
                    config: {
                        allowedEids: ["*"],
                        mode: "auth",
                        showTitle: false,
                    },
                    token: token,
                    callback: async function (data: any) {
                        if (data.success) {
                            const valid_token = await validateTokenAction(data.token, token)
                            console.log(data)
                            console.log(valid_token)
                            if (valid_token) {
                                handleSuccess(data)
                                window.coreid_client.close()
                            } else {
                                handleError(data)
                                window.coreid_client.close()
                            }
                        } else if (data.type === "failed") {
                            console.log("authentication failed", data.errorMessage)
                            window.coreid_client.close()
                        } else if (data.type === "cancelled") {
                            console.log("user closed the app")
                            window.coreid_client.close()
                        } else if (data.type === "error") {
                            console.log("an error occurred", data.errorMessage)
                            window.coreid_client.close()
                        }
                    },
                })

                window.coreid_client.start()
            }
        }

        return () => {
            document.body.removeChild(script)
        }
    }, [token])

    function handleSuccess(response: any) {
        setSuccessState("Authenticated")
        console.log("Token validation successful", response)
    }

    function handleError(error: any) {
        console.log("Error validating token", error)
    }

    return (
        <div>
            <h1>After a few seconds an iframe should appear</h1>
            <br/>
            <h3>User Authentication state - {successState}</h3>
        </div>
    )
}
