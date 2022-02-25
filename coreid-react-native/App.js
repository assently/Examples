import {StatusBar} from 'expo-status-bar';
import React, {useState, useRef} from 'react';
import {StyleSheet, View, Button, Linking} from 'react-native';
import {WebView} from 'react-native-webview';

const coreIdStyle = (show) => {
    return {
        height: show ? 400 : 0,
        backgroundColor: 'black',
        width: 320
    }
};

// This sample is provided AS IS without any guarantees. It is provided to give developers an idea of how to approach
// integrating CoreID in React Native, and is **not** a ready-made implementation.
//
// Reference our documentation: https://docs.assently.com/coreid/
const config = {
    // coreIdScriptUrl: must match the environment you are targeting (test / prod)
    coreIdScriptUrl: "https://coreid-test.assently.com/embed/coreid.js",
    // coreIdReturnUrl: Will be sent with the identification request to BankID, typically your app url.
    coreIdReturnUrl: "exp:////127.0.0.1:19000", // URL scheme must be present in originWhitelist below
    // authorizationToken: Token used to initialize the authorization, must be generated serverside on-demand (see docs).
    authorizationToken: "<your token>",
    // domain: On Android, this sets the baseUrl of the WebView. It affects CORS and must match the "hst" value of the authorizationToken.
    domain: "<your domain>",
}

const run = `
    coreid_client.start();
    true;
`;

export default function App() {

    const [showWebView, setShowWebView] = useState(false);
    const webref = useRef(null);

    const shouldNavigate = (p)=>{
        const {url, loading} = p;
        if (url.includes("bankid://") && loading) {
            Linking.openURL(url);
            return false;
        }
        return true;
    }

    const onMessage = (event) => {
        const {data} = event.nativeEvent;
        console.log(data);
    };

    const html = `
        <html>
            <head>
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <style>iframe.coreid-embed-iframe { height: 100%; }</style>
            </head>
            <body>
                <div id="coreid_container"></div>
                <script src="${config.coreIdScriptUrl}"></script>
                <script>
                    coreid_client.init({
                        injectIntoNode:"#coreid_container",
                        config: {
                            "allowedEids": ["se-bankid"],
                            "mode": "auth",
                            "language": "en",
                            "provider": "se-bankid",
                            "providerSettings": {
                                "se-bankid":{
                                    autoStart:true,
                                    redirectMode: "always",
                                    redirectUrl: "${config.coreIdReturnUrl}"
                                }
                            }
                        },
                        callback: function(data) {
                            window.ReactNativeWebView.postMessage(JSON.stringify(data));
                            console.log('CoreID response', data);
                        },
                        token: "${config.authorizationToken}"
                    });
                </script>
            </body>
        </html>`;

    return (
        <View style={styles.container}>
            <Button
                title="Start CoreID"
                onPress={() => {
                    setShowWebView(true);
                    webref.current.injectJavaScript(run);
                }}/>
            <View style={coreIdStyle(showWebView)}>
                <WebView
                    ref={webref}
                    originWhitelist={['https://*', 'bankid://*']}
                    onShouldStartLoadWithRequest={shouldNavigate}
                    onMessage={onMessage}
                    source={{
                        baseUrl: config.domain,
                        html: html
                    }}
                />
            </View>
            <StatusBar style="auto"/>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
    },
});
