export default {
    add_app_alert(context, appAlert) {
        context.commit("add_app_alert", appAlert);
    },
    remove_app_aplert(context, index) {
        context.commit("remove_app_alert", index);
    },
    reset_appAlerts(context) {
        context.commit("reset_app_alerts");
    },
};
