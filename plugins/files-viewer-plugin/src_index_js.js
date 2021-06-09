/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
exports.id = "src_index_js";
exports.ids = ["src_index_js"];
exports.modules = {

/***/ "./src/index.js":
/*!**********************!*\
  !*** ./src/index.js ***!
  \**********************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

eval("const pkg = __webpack_require__(/*! ../package.json */ \"./package.json\");\n\nconst path = __webpack_require__(/*! path */ \"path\");\n\nmodule.exports = {\n  name: pkg.name,\n  path: `/${pkg.name}.js`,\n  handler: (req, res) => {\n    res.sendFile(path.resolve(__dirname, \"plugin.js\")); // Set disposition and send it.\n  }\n};\n\n//# sourceURL=webpack://files-viewer-plugin/./src/index.js?");

/***/ }),

/***/ "./package.json":
/*!**********************!*\
  !*** ./package.json ***!
  \**********************/
/***/ ((module) => {

"use strict";
eval("module.exports = JSON.parse(\"{\\\"name\\\":\\\"files-viewer-plugin\\\",\\\"version\\\":\\\"0.0.1\\\",\\\"main\\\":\\\"index.js\\\",\\\"devDependencies\\\":{\\\"@babel/core\\\":\\\"7.14.3\\\",\\\"@babel/preset-react\\\":\\\"7.13.13\\\",\\\"babel-loader\\\":\\\"8.2.2\\\",\\\"clean-webpack-plugin\\\":\\\"^3.0.0\\\",\\\"html-webpack-plugin\\\":\\\"5.3.1\\\",\\\"webpack\\\":\\\"5.14.0\\\",\\\"webpack-cli\\\":\\\"4.5.0\\\"},\\\"scripts\\\":{\\\"build\\\":\\\"webpack\\\",\\\"watch\\\":\\\"webpack --watch src\\\"},\\\"dependencies\\\":{\\\"js-plugin\\\":\\\"^1.0.8\\\",\\\"react\\\":\\\"^17.0.2\\\",\\\"styled-components\\\":\\\"^5.3.0\\\"}}\");\n\n//# sourceURL=webpack://files-viewer-plugin/./package.json?");

/***/ })

};
;