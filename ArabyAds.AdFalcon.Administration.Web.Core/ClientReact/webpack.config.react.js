const path = require('path');
const merge = require('webpack-merge');
const commonConfig = require('./webpack.config.common');

module.exports = merge(commonConfig(), {
    entry: {
     
    },
    output: {
        filename: '[name].[hash].build.js',
        path: path.resolve(__dirname, '../wwwroot/ClientReactJs')
    }
});
