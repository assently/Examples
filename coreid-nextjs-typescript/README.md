# Integrating coreID into NextJS using Typescript. 
This a minimal example on how to integrate CoreId into a next/react project. 

# Requirements
- This requires a way to encode validate JWT tokens. I use Auth0's `jsonwebtoken` package
- A way to generate uuid4 id's I use `uuid4`.
- Next 14 Server actions or API routes to encode and validate your tokens on the server to ensure you don't leak secrets.
- General understanding how JWT's work.

# What this does do.
This does not save any user data for you, it will be upto the developer to decide what to use and how to create a user session
after a successful response. In our situation we used the authentication success to confirm that the user was able to make a 
purchase on our platform but your requirements may vary. This mostly just shows an overview of how to setup CoreID in 
a modern web framework.
