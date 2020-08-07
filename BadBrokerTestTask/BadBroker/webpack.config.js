var path = require("path");
var MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: "./ClientApp/index.tsx",
    output: {
        path: path.resolve(__dirname, "wwwroot/dist"),
        filename: "index.js",
        publicPath: "/"
    },
    resolve: {
        extensions: [".ts", ".tsx", ".js"]
    },
    devtool: "source-map",
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                loader: "ts-loader"
            },
            {
                test: /\.(less)$/,
                use: [
                    "style-loader",
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    "less-loader"
                ],
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "style.css"
        })
    ]
};