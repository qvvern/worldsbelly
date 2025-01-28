export default {
  add_app_alert(state, appAlert) {
      state.appAlerts.push(appAlert);
  },
  remove_app_alert(state, index) {
      state.appAlerts.splice(index, 1);
  },
  reset_app_alerts(state) {
      state.appAlerts = [];
  },
  set_appError(state, error) {
      state.appError = error;
  },
  reset_appError(state) {
      state.appError = null;
  },
}
