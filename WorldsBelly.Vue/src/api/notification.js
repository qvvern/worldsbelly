import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";


function fetchNotifications(startAt) {
	return axios
		.get(domain + `/api/Notifications?startAt=${startAt ? startAt : 0}&amount=50`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function fetchNotification(id) {
	return axios
		.get(domain + `/api/Notifications/${id}`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}
function deleteNotifications() {
	return axios
		.delete(domain + `/api/Notifications`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function readNotification(id) {
	return axios
		.put(domain + `/api/Notifications/${id}`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function readNotifications() {
	return axios
		.put(domain + `/api/Notifications`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}


export default {
  fetchNotifications,
  fetchNotification,
  deleteNotifications,
  readNotification,
  readNotifications
}
