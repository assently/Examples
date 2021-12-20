import {StatusBar} from 'expo-status-bar';
import React, {useState, useRef} from 'react';
import {StyleSheet, View, Button} from 'react-native';
import {WebView} from 'react-native-webview';

const coreIdStyle = (show) => {
    return {
        height: show ? 400 : 0,
        backgroundColor: 'black',
        width: 320
    }
};

const run = `
    coreid_client.start();
    true;
`;

export default function App() {

    const [showWebView, setShowWebView] = useState(false);
    const webref = useRef(null);

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
                originWhitelist={['*']}
                source={{
                    html: '<html>\n' +
                        '\n' +
                        '<head> \n' +
                        '<meta name="viewport" content="width=device-width, initial-scale=1.0">\n'+
                        '<style>iframe.coreid-embed-iframe { height: 100%; }</style>\n'+
                        '</head>\n' +
                        '\n' +
                        '<body> \n' +
                        '<div id="coreid_container"></div> \n' +
                        '<script src="https://coreid-test.assently.com/embed/coreid.js"></script> \n' +
                        '<script> \n' +
                        'coreid_client.init({ \n' +
                        'injectIntoNode:"#coreid_container", \n' +
                        'config: { \n' +
                        '"allowedEids": ["se-bankid"], \n' +
                        '"mode": "auth", \n' +
                        '"language": "en", \n' +
                        '"provider": "se-bankid", \n' +
						'"providerSettings": {\n' +
						'"se-bankid":{' +
                        'autoStart:false,' +
                        'redirectMode:"always",' +
                        'redirectUrl:"exp://127.0.0.1:19000"' +
                        '}\n' +
						'}\n' +
                        '},\n' +
                        'token: \'<AUTH TOKEN HERE>\' \n' +
                        '}); \n' +
                        '</script> \n' +
                        '</body>\n' +
                        '\n' +
                        '</html>'
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
