const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");


module.exports = {
    entry: {
        site: './src/ts/site.ts',
        index: './src/ts/index.ts',
        validation: './src/ts/validation.ts',
        initializeinstallation: './src/ts/InitializeInstallation.ts',
        circularflow: './src/ts/flow/CircularFlow.ts',
        multiplechoice: './src/ts/flow/slide/MultipleChoice.ts',
        singlechoice: './src/ts/flow/slide/SingleChoice.ts',
        rangequestion: './src/ts/flow/slide/RangeQuestion.ts',
        infoslide: './src/ts/flow/slide/InfoSlide.ts',
        openquestion: './src/ts/flow/slide/OpenQuestion.ts',
        subtheme: './src/ts/flow/slide/SubThemes.ts',
        inputhandler: './src/ts/flow/InputHandler.ts',
        votetablehandler: './src/ts/flow/VoteTableHandler.ts',
        slideEditor: './src/ts/flow/slide/SlideEditor.ts',
        formatProject: './src/ts/Project/formatProject.ts',
        qr : './src/ts/qr/qr.ts',
        searchBar : './src/ts/users/searchbar.ts',
        project :'./src/ts/Project/Project.ts',
        createSlide: './src/ts/Slide/CreateSlide.ts',
        flowBeheer: './src/ts/flow/FlowBeheer.ts'
    },
    output: {
        filename: '[name].entry.js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist'),
        clean: true
    },
    devtool: 'source-map',
    mode: 'development',
    resolve: {
        extensions: [".ts", ".js"], 
        extensionAlias: {'.js': ['.js', '.ts']}
    },
    module: {
        rules: [
            {
                test: /\.ts$/i, 
                use: ['ts-loader'], 
                exclude: /node_modules/
            },
            {
                test: /\.s?css$/,
                use: [{ loader: MiniCssExtractPlugin.loader }, 'css-loader', 'sass-loader']
            },
            {
                test: /\.(png|svg|jpg|jpeg|gif|webp)$/i,
                type: 'asset'
            },
            {
                test: /\.(eot|woff(2)?|ttf|otf|svg)$/i,
                type: 'asset'
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css"
        })
    ]
};
