//const pkg = require("../package.json");
(function (Plugins, window, document, undefined) {
  if (!Plugins) {
    Plugins = [];
  }

  Plugins.push({
    name: "files-viewer-plugin",
    files: {
      context: {
        processOptions(options, item) {
          options.push({
            key: `files-viewer-plugin-options`,
            label: "Display file info! (Plugin 2)",
            icon: "images/tick.rounded.svg",
            onClick: (e) => {
              const { title, contentLength } = item;
              toastr.info(`'${title}' (${contentLength})`);
            },
            disabled: false,
          });
        },
      },
    },
  });
})((window.Plugins = window.Plugins || []), window, document);
// const pkg = require("../package.json");

// module.exports = {
//   register: () => {
//     return {
//       name: pkg.name,
//       files: {
//         context: {
//           processOptions(options, item) {
//             options.push({
//               key: `${pkg.name}-options`,
//               label: "Display file info! (Plugin 2)",
//               icon: "images/tick.rounded.svg",
//               onClick: (e) => {
//                 const { title, contentLength } = item;
//                 toastr.info(`'${title}' (${contentLength})`);
//               },
//               disabled: false,
//             });
//           },
//         },
//       },
//     };
//   },
// };
