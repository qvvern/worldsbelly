import api from '@/api/notification'
import i18n from "@/lang";
function buildNotification(item) {
  var notification = {
    id: item.id,
    createdAt: item.createdAt,
    readAt: item.readAt,
    isRead: item.isRead,
    title: item.title.replace("[SenderId]", `<b>${item.sender}</b>`),
    content: item.content.replace("[SenderId]", `<a style="text-decoration: none" href="${location.origin}/profile/${item.sender}/recipes"><b>${item.sender}</b></a>`),
    sender: item.sender
  }
  if(item.action) {
    notification.content = notification.content.replace("[Action]", `<a href="${location.origin}${item.action}">${i18n.t('actionLabel')}</a>`)
  }
  return notification;
}

export default {
	async fetch_notifications({ commit }) {
		try {
			const response = await api.fetchNotifications();
      if(response && response.length > 0) {
        var notifications = response.map((notification) => {
          return buildNotification(notification);
        })
        commit('set_notifications', notifications)
      }
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_append_notifications({ commit, getters }) {
		try {
			const response = await api.fetchNotifications(getters.get_notifications.length);
      if(response && response.length > 0) {
        var notifications = response.map((notification) => {
          return buildNotification(notification);
        })
        commit('append_notifications', notifications)
      }
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_notification({ commit }, id) {
		try {
			const response = await api.fetchNotification(id);
      if(response) {
        var notification = buildNotification(response);
        commit('prepend_notification', notification)
      }
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async delete_notifications({ commit }) {
		try {
			await api.deleteNotifications();
      commit('set_notifications', [])
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async read_notification({ commit }, id) {
		try {
			await api.readNotification(id);
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async read_notifications({ commit }) {
		try {
			await api.readNotifications();
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
}
