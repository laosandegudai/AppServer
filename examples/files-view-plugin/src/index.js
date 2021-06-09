const pkg = require("../package.json");
const path = require("path");

module.exports = {
  name: pkg.name,
  path: `/${pkg.name}.js`,
  handler: (req, res) => {
    res.sendFile(path.resolve(__dirname, "plugin.js")); // Set disposition and send it.
  },
};
