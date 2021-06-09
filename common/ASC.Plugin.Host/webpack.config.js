const ModuleFederationPlugin = require("webpack/lib/container/ModuleFederationPlugin");
const path = require("path");
const pkg = require("./package.json");

module.exports = {
  mode: "development",
  target: "node",
  entry: "./index.js",
  externals: [path.resolve(__dirname, `${pkg.pluginsDir}`)],
  plugins: [
    new ModuleFederationPlugin({
      name: "host",
      filename: "remoteEntry.js",
      remotes: {},
      exposes: {},
      shared: [],
    }),
  ],
};
