import CrmStore from "./CrmStore";
import TreeFoldersStore from "./TreeFoldersStore";
import ContactsStore from "./ContactsStore";
import FilterStore from "./FilterStore";

const crmStore = new CrmStore();
const treeFoldersStore = new TreeFoldersStore();
const contactsStore = new ContactsStore();
const filterStore = new FilterStore();

const stores = {
  crmStore,
  treeFoldersStore,
  contactsStore,
  filterStore,
};

export default stores;
