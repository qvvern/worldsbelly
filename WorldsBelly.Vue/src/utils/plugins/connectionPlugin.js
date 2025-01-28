import * as signalR from "@microsoft/signalr";
import { domain } from "@/utils/constants/ApiConstants.js";

/* eslint-disable */
const connectionPlugin = {
    install(Vue) {
        Vue.prototype.$connection = {};

        Vue.prototype.$connection.build = function() {
            const connection = new signalR.HubConnectionBuilder()
                .configureLogging(signalR.LogLevel.Information)
                .withUrl(domain+"/worldsbelly", {
                    withCredentials: false
                }).build();

            connection.onclose(() => {
                console.log("Connection closed");
            });

            return connection;
        };
    }
};

export default connectionPlugin;
