export default {
  set_notifications(state, notifications) {
		state.notifications = notifications
	},
  append_notifications(state, notifications) {
    state.notifications = state.notifications.concat(notifications);
	},
  prepend_notification(state, notification) {
    state.notifications.unshift(notification);
	},
}
