import { action, computed, makeObservable, observable } from "mobx";
import api from "@appserver/common/api";
import FilterStore from "./FilterStore";

const { CrmFilter } = api;

class ContactsStore {
  contacts = [];
  filterStore = null;

  constructor() {
    this.filterStore = new FilterStore();
    makeObservable(this, {
      contacts: observable,
      getContactsList: action,
      contactsList: computed,
    });
  }

  getContactsList = async (filter) => {
    let filterData = filter && filter.clone();

    if (!filterData) {
      filterData = CrmFilter.getDefault();
    }

    const res = await api.crm.getContactsList(filterData);
    filterData.total = res.total;
    this.filterStore.setFilterParams(filterData);
    this.setContacts(res.items);
  };

  setContacts = (contacts) => {
    this.contacts = contacts;
  };

  get contactsList() {
    const list = this.contacts.map((contact) => {
      const {
        about,
        accessList,
        canDelete,
        canEdit,
        commonData,
        createdBy,
        created,
        currency,
        displayName,
        firstName,
        lastName,
        haveLateTasks,
        id,
        industry,
        isCompany,
        isPrivate,
        isShared,
        mediumFotoUrl,
        shareType,
        smallFotoUrl,
        title,
      } = contact;

      return contact;
    });

    return list;
  }
}

export default ContactsStore;
