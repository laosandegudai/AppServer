const ModuleFederationPlugin = require("webpack/lib/container/ModuleFederationPlugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const path = require("path");
const pkg = require("./package.json");

module.exports = {
  mode: "development",
  target: "node",
  entry: {
    index: "./src/index.js",
    plugin: "./src/plugin.js",
  },
  output: {
    path: path.resolve(__dirname, `../../plugins/${pkg.name}`),
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        loader: "babel-loader",
        exclude: /node_modules/,
        options: {
          presets: ["@babel/preset-react"],
        },
      },
    ],
  },
  plugins: [
    new CleanWebpackPlugin(),
    new ModuleFederationPlugin({
      name: pkg.name,
      library: { type: "commonjs" },
      filename: "remoteEntry.js",
      exposes: {
        "./plugin": "./src/index",
      },
      shared: require("./package.json").dependencies,
    }),
  ],
};
