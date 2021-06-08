import CrmStore from "./CrmStore";
import TreeFoldersStore from "./TreeFoldersStore";
import ContactsStore from "./ContactsStore";

const crmStore = new CrmStore();
const treeFoldersStore = new TreeFoldersStore();
const contactsStore = new ContactsStore();

const stores = {
  crmStore,
  treeFoldersStore,
  contactsStore
};

export default stores;
