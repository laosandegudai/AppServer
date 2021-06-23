import authStore from "@appserver/common/store/AuthStore";
import PaymentStore from "./PaymentStore";
import WizardStore from "./WizardStore";
import SettingsSetupStore from "./SettingsSetupStore";
import ConfirmStore from "./ConfirmStore";
import ModuleStore from "./ModuleStore";

const paymentStore = new PaymentStore();
const wizardStore = new WizardStore();
const setupStore = new SettingsSetupStore();
const confirmStore = new ConfirmStore();
const moduleStore = new ModuleStore();

const store = {
  auth: authStore,
  payments: paymentStore,
  wizard: wizardStore,
  setup: setupStore,
  confirm: confirmStore,
  module: moduleStore,
};

export default store;
