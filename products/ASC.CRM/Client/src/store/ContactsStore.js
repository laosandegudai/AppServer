import { makeAutoObservable } from "mobx";
import { getContacts } from "@appserver/common/api/crm";

class ContactsStore {
  contacts = [];

  constructor() {
    makeAutoObservable(this);
  }

  fetchContacts = async () => {
    const contacts = await getContacts();
    this.setContacts(contacts);

    return contacts;
  };

  setContacts = (contacts) => {
    this.contacts = contacts;
  };
}

export default ContactsStore;
