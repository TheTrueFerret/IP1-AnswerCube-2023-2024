const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");


module.exports = {
    entry: {
        site: './src/js/site.ts', 
        validation: './src/js/validation.ts', 
        index: './src/js/index.ts',
        slide: './src/js/slide/slide.ts',
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
