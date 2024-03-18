const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");


module.exports = {
    entry: {
        site: './src/ts/site.ts', 
        index: './src/ts/index.ts',
        multiplechoice: './src/ts/flow/slide/MultipleChoice.ts',
        qr : './src/ts/qr/qr.ts',
    },
    output: {
        filename: '[name].entry.js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist')
    },
    devtool: 'source-map',
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [{ loader: MiniCssExtractPlugin.loader }, 'css-loader'],
            },
            {
                test: /\.(eot|woff(2)?|ttf|otf|svg)$/i,
                type: 'asset'
            },
        ]
    }, 
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css"
        })
    ]
};
