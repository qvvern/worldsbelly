const path = require('path')

module.exports = {
  transpileDependencies: [
    'vuetify'
  ],
	devServer: {
    hot: false,
    liveReload: true,
    port: 8080,
    headers: {
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Methods": "GET, POST, PUT, DELETE, PATCH, OPTIONS",
      "Access-Control-Allow-Headers": "X-Requested-With, content-type, Authorization",
    },
		proxy: {
			'/api': {
				target: 'https://localhost:5001/api',
				pathRewrite: { '^/api': '' },
			},
		},
	},
	pluginOptions: {
		'style-resources-loader': {
			preProcessor: 'scss',
			patterns: [
				// path.resolve(__dirname, 'src/assets/styles/variables.scss'),
			],
		},
	},
	runtimeCompiler: true,
}
