# Using CoreID with Auth0 through SAML

This setup uses a hosted version of CoreID, thus require no code changes.

Read up on the documentation on https://docs.assently.com/coreid/hosted/

### Step 1: Download SAML metadata

For test/dev download it from  https://eid-gateway-test.assently.com/metadata

This file contains all the information you need. 

### Step 2 (optional): Create a new tenant at Auth0 if you haven't already. 

### Step 3: Create a new SAML enterprise connection.

In the form you will fill out:

- Connection name: Give it a brief descriptive name, like "assently-coreid"
- Sign in URL: Get this from Step 1, in md:SingleSignOnService:Location 
- Sign out URL: Get this from Step 1, in md:SingleLogoutService:Location
- Certificate: Copy the content of `ds:X509Certificate` into sample-certificate-file.crt (keeping the begin- and end-tags), upload this to Auth0 in the form.

### Step 4: Compile information to send to Assently

We'll be able to retrieve the information we need through the Auth0 metadata-endpoint, https://YOUR_DOMAIN/samlp/metadata?connection=YOUR_CONNECTION_NAME

Edit the above link and verify that it works, and send it to your contact at Assently. Please note if your Auth0 is set up with custom domain.

Example: https://my-auth0-dev-tenant.eu.auth0.com/samlp/metadata?connection=assently-coreid-test

### Step 4.5: Wait for confirmation

Your Assently contact will verify once your SAML-metadata has been added and is ready to be used.

### Step 5: Add Mappings

To fill in the user-profiles in Auth0 correctly, copy-paste contents of `mapping.json` into the Mappings-section of your SAML Connection settings at Auth0.

### Step 6: Enable and customize

Enable the connection on your application and try it out. You can customize background image, logo and identity providers of the hosted CoreID by contacting us.

### Production

Read up on the documentation on https://docs.assently.com/coreid/hosted/

To set up for production, repeat the above process with metadata for production. It should be a different Auth0-tenant for Prod.