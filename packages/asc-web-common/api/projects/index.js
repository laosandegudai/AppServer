import * as fakeProjects from "./fake";
export function getFolders(fake = true) {
  if (fake) {
    return fakeProjects.getFolders();
  }
}

export function getFilterProjects(fake = true) {
  if (fake) {
    return fakeProjects.getFilterProjects();
  }
}
