import * as fakeProjects from "./fake";
export function getFolders(fake = true) {
  if (fake) {
    return fakeProjects.getFolders();
  }
}
