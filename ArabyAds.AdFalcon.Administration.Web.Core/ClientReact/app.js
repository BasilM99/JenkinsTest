// app.js

const server = require('server');

const { get, post } = server.router;

// Launch server
server({ port: 5008 }, [
    get('/', ctx => 'Hello world!')
]);