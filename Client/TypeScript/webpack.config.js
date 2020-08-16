const path = require('path');
const AltvWebpackPlugin = require('altv-webpack-plugin');
const webpack = require("webpack");

module.exports = {
    entry: "./src/index.ts",
    devtool: "source-map",
    target: "webworker",
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: "ts-loader",
                exclude: "/node_modules/"
            }
        ]
    },
    resolve: {
        extensions: [".tsx", ".ts", ".js"],
        modules: ["node_modules"]
    },
    output: {
        filename: "main.js",
        path: path.resolve(__dirname, "dist"),
    },
    plugins: [
        new AltvWebpackPlugin()
    ]
}
