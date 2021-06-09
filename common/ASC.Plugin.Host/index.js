//import * as fs from "fs";
//import loadPlugins from "./loadPlugins.js";
import loadPlugins from "mf-plugins";
const express = require("express");
const path = require("path");

//import config from "./package.json";
const __dirname = path.join();
const port = 5025;
const pluginArray = [];
const basePath = "/plugins";
const pluginsDir = path.resolve(__dirname, "../../plugins");
const remoteEntry = "remoteEntry.js";

const app = express();
app.use(express.static("public"));

app.get(basePath, (req, res) => {
  let response = `Next plugins are available:<br>`;
  let pluginsBlock = "";
  for (let index = 0; index < pluginArray.length; index++) {
    const pluginUrl = pluginArray[index];
    pluginsBlock += `<li><a href="${basePath}${pluginUrl}">${pluginUrl}</a></li>`;
  }

  if (pluginArray.length > 0) {
    response += `<ul>${pluginsBlock}</ul>`;
  }

  const html = `<!DOCTYPE html><html><body>${response}</body></html>`;

  res.send(html);
  //res.sendFile(path.resolve(__dirname, "public", "index.html"));
});

// app.listen(port, async () => {
//   console.log(`Example app listening at http://localhost:${port}/plugins`);
//   console.log(loadPlugins);

//   const plugins = await loadPlugins(pluginsDir);
//   plugins.forEach((plugin) => {
//     pluginArray.push(plugin.path);
//     app.get(`/plugins${plugin.path}`, plugin.handler);
//   });

//   fs.readdirSync(pluginsDir).map(async (d) => {
//     let rmEntryPath = path.resolve(`${pluginsDir}/${d}/${remoteEntry}`);
//     console.log(rmEntryPath);

//   });
// });

const start = async () => {
  const plugins = await loadPlugins(pluginsDir);
  plugins.forEach((plugin) => {
    pluginArray.push(plugin.path);
    app.get(`/plugins${plugin.path}`, plugin.handler);
  });

  app.listen(port, () => {
    console.log(`Example app listening at http://localhost:${port}/plugins`);
  });
};

start();
