


module.exports = function (grunt) {


    // Time your grunt tasks and never need to loadGruntTask again
    require('time-grunt')(grunt);
    require('load-grunt-tasks')(grunt);

    const autoprefixer = require('autoprefixer');
    const webpack = require("webpack");
    const path = require('path');


    const BUILD_MODE = 'development';//production||development
    const BUILD_DIR = 'D:\\github\\dsp-platform\\src-netcore\\AdFalcon.Web\\Noqoush.AdFalcon.Web\\Noqoush.AdFalcon.Web.Core\\wwwroot\\Scripts\\js\\';
    const APP_DIR = path.resolve(__dirname, 'src');

    process.env.BABEL_ENV = process.env.NODE_ENV = BUILD_MODE;

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        project: {
            static: 'public',
            dev: 'src',
            dist: BUILD_DIR,
            build: '<%= grunt.template.today("yyyymmdd") %>'
        },
        connect: {
            server: {
                options: {
                    useAvailablePort: true,
                    host: 'localhost',
                    port: process.env.PORT || '5000',
                    base: BUILD_DIR,
                    livereload: false,
                    open: true
                }
            }
        },
        copy: {
            setup: {
                expand: true,
                cwd: '<%= project.static %>',
                src: ['*', '*/**'],
                dest: '<%= project.dist %>'
            }
        },
        sass: {
            dist: {
                files: [{
                    expand: true,
                    cwd: 'public/css',
                    src: ['*.scss'],
                    dest: 'public/css',
                    ext: '.css'
                }]
            }
        },
        jshint: {
            options: {
                reporter: require('jshint-stylish')
            },
            dev: [
                '<%= project.dev %>/**/*.jsx',
                '<%= project.dev %>/**/*.js']
        },
        watch: {
            scripts: {
                files: [
                    '<%= project.dev %>/**/*.jsx',
                    '<%= project.dev %>/**/*.js',
                    '<%= project.static %>/js/global.js'],
                tasks: ['webpack:build', 'jshint:dev'], // !important
                options: {
                    livereload: true,
                    reload: true
                }
            }
        },
        // webpack !pay attention to this task!
        webpack: {
            build: {
                mode: BUILD_MODE,
                entry: [require.resolve('./config/polyfills'), "./src/index.js"],
                output: {
                    path: BUILD_DIR,
                    filename: 'js/build.js',
                },
                stats: {
                    colors: false,
                    modules: true,
                    reasons: true
                },
                storeStatsTo: 'webpackStats',
                progress: true,
                failOnError: true,
                watch: true,
		
                module: {
                    rules: [
                        {
                            test: /\.(js|jsx|mjs)$/,
                            include: APP_DIR,
                            exclude: /(node_modules|bower_components)/,
                            loader: "babel-loader",
                            options: {
                                presets: ['env'],
                                babelrc: true
                            }
                        }, {
                            // "oneOf" will traverse all following loaders until one will
                            // match the requirements. When no loader matches it will fall
                            // back to the "file" loader at the end of the loader list.
                            oneOf: [
                                // "url" loader works like "file" loader except that it embeds assets
                                // smaller than specified limit in bytes as data URLs to avoid requests.
                                // A missing `test` is equivalent to a match.
                                {
                                    test: [/\.bmp$/, /\.gif$/, /\.jpe?g$/, /\.png$/],
                                    loader: require.resolve('url-loader'),
                                    options: {
                                        limit: 10000,
                                        name: 'img/[name].[hash:8].[ext]',
                                    },
                                },
                                // Process JS with Babel.
                                {
                                    test: /\.(js|jsx|mjs)$/,
                                    include: APP_DIR,
                                    loader: require.resolve('babel-loader'),
                                    options: {
                                        // @remove-on-eject-begin
                                        babelrc: false,
                                        presets: [require.resolve('babel-preset-react-app')],
                                        // @remove-on-eject-end
                                        // This is a feature of `babel-loader` for webpack (not Babel itself).
                                        // It enables caching results in ./node_modules/.cache/babel-loader/
                                        // directory for faster rebuilds.
                                        cacheDirectory: true,
                                    },
                                },
                                // "postcss" loader applies autoprefixer to our CSS.
                                // "css" loader resolves paths in CSS and adds assets as dependencies.
                                // "style" loader turns CSS into JS modules that inject <style> tags.
                                // In production, we use a plugin to extract that CSS to a file, but
                                // in development "style" loader enables hot editing of CSS.
                                {
                                    test: /\.(css|scss)$/,
                                    use: [
                                        require.resolve('style-loader'),
                                        {
                                            loader: require.resolve('css-loader'),
                                            options: {
                                                importLoaders: 1,
                                            },
                                        },
                                        {
                                            loader: require.resolve('postcss-loader'),
                                            options: {
                                                // Necessary for external CSS imports to work
                                                // https://github.com/facebookincubator/create-react-app/issues/2677
                                                ident: 'postcss',
                                                plugins: () => [
                                                    require('postcss-flexbugs-fixes'),
                                                    autoprefixer({
                                                        browsers: [
                                                            '>1%',
                                                            'last 4 versions',
                                                            'Firefox ESR',
                                                            'not ie < 9', // React doesn't support IE8 anyway
                                                        ],
                                                        flexbox: 'no-2009',
                                                    }),
                                                ],
                                            },
                                        },
                                        {
                                            loader: "sass-loader"
                                        }
                                    ],
                                },
                                // "file" loader makes sure those assets get served by WebpackDevServer.
                                // When you `import` an asset, you get its (virtual) filename.
                                // In production, they would get copied to the `build` folder.
                                // This loader doesn't use a "test" so it will catch all modules
                                // that fall through the other loaders.
                                {
                                    // Exclude `js` files to keep "css" loader working as it injects
                                    // its runtime that would otherwise processed through "file" loader.
                                    // Also exclude `html` and `json` extensions so they get processed
                                    // by webpacks internal loaders.
                                    exclude: [/\.(js|jsx|mjs)$/, /\.html$/, /\.json$/],
                                    loader: require.resolve('file-loader'),
                                    options: {
                                        name: '/fonts/[name].[hash:8].[ext]',
                                    },
                                },
                            ],
                        },
                        // ** STOP ** Are you adding a new loader?
                        // Make sure to add the new loader(s) before the "file" loader.
                    ]
                },
                resolve: { extensions: ['*', '.js', '.jsx'] }
                // ,
                // plugins: [new webpack.HotModuleReplacementPlugin()]
            }
        }
    });


    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-sass');
    grunt.loadNpmTasks('grunt-contrib-connect');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-jshint');


    // Tasks
    grunt.registerTask('default', ['start']);

    grunt.registerTask('build', ['copy:setup', 'webpack:build', 'jshint:dev']);

    grunt.registerTask('start', [
        // 'connect:server',
        'build',
        'watch'
    ]);




};
