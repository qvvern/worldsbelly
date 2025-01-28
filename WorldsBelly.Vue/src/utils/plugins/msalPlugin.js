/* eslint-disable */
import * as msal from "@azure/msal-browser";
import store from "@/store/store";
import router from '@/router'
const msalConfig = {
  auth: {
      clientId: "9db55ba2-55dc-4ec7-b05f-368417756ee8",
      authority: "https://worldsbelly.b2clogin.com/worldsbelly.onmicrosoft.com/B2C_1_signin_signup/",
      knownAuthorities: ["worldsbelly.b2clogin.com"],
  },
  cache: {
      cacheLocation: "sessionStorage", // This configures where your cache will be stored
      storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
  },
  system: {
      // loggerOptions: {
      //     loggerCallback: (level, message, containsPii) => {
      //         if (containsPii) {
      //             return;
      //         }
      //         switch (level) {
      //             case msal.LogLevel.Error:
      //                 console.error(message);
      //                 return;
      //             case msal.LogLevel.Info:
      //                 console.info(message);
      //                 return;
      //             case msal.LogLevel.Verbose:
      //                 console.debug(message);
      //                 return;
      //             case msal.LogLevel.Warning:
      //                 console.warn(message);
      //                 return;
      //         }
      //     }
      // }
  }
};
const msalInstance = new msal.PublicClientApplication(msalConfig);


const msalPlugin = {
  install(Vue) {
    Vue.prototype.$msal = Vue.observable({
      isMsalAuthenticated: getIsMsalAuthenticated(),
      isAuthenticated: getIsAuthenticated()
    });

    Vue.prototype.$msal.signInRedirect = async function () {
      const loginRequest = {
          scopes: ["openid", "profile", "offline_access", "https://worldsbelly.onmicrosoft.com/e4c13107-70ab-4b81-9169-6d0170fb595f/api/read"],
      };

      return await msalInstance.loginRedirect(loginRequest)
    };

    Vue.prototype.$msal.signOut = async function () {
      await msalInstance.logoutRedirect();
      sessionStorage.removeItem('signedinuser');
      Vue.prototype.$msal.isMsalAuthenticated = false;
    };

    Vue.prototype.$msal.acquireTokenRedirect = async function () {
      const request = {
          account: msalInstance.getAllAccounts()[0],
          scopes: ["https://worldsbelly.onmicrosoft.com/e4c13107-70ab-4b81-9169-6d0170fb595f/api/read"]
      };
      return await msalInstance.acquireTokenSilent(request)
          .catch(error => {
              console.warn("silent token acquisition fails. acquiring token using redirect");
              Vue.prototype.$msal.isMsalAuthenticated = false;
              Vue.prototype.$msal.signOut();
              if (error instanceof msal.InteractionRequiredAuthError) {
                  // fallback to interaction when silent call fails
                  return msalInstance.acquireTokenRedirect(request)
                      .catch(error => {
                        Vue.prototype.$msal.isMsalAuthenticated = false;
                        // Vue.prototype.$msal.signOut();
                          console.error(error);
                      });
              } else {
                  console.warn(error);
              }
      });
    };
    Vue.prototype.$msal.authorize = async function () {
      var isValid = await getIsAuthenticated();
      Vue.prototype.$msal.isAuthenticated = isValid;
      return isValid;
    };

    Vue.prototype.$msal.getProfile = function () {
      if(getIsMsalAuthenticated()) return msalInstance.getAllAccounts()[0];
    };

    function getIsMsalAuthenticated() {
      var accounts = msalInstance.getAllAccounts();
      return accounts && accounts.length > 0;
    };

    async function getIsAuthenticated() {
      var accounts = msalInstance.getAllAccounts();
      var isValid = accounts && accounts.length > 0;
      if(isValid) {
          await store.dispatch("user/fetch_active_user").then((response) => {
            if(response) return isValid
            return false;
          }).catch(() => {
            return false
          })
        }
      return isValid;
    };

    var handleRedirect = async function () {
      await msalInstance.handleRedirectPromise().then(async (response) => {
        if (response !== null) {
            Vue.prototype.$msal.isMsalAuthenticated = getIsMsalAuthenticated();
            if(Vue.prototype.$msal.isMsalAuthenticated) {
              store.dispatch("user/fetch_or_create_user").then(async () => {
                Vue.prototype.$msal.isAuthenticated = await getIsAuthenticated();
              }).catch(() => {
                Vue.prototype.$msal.signOut();
              })
            }
        }
      }).catch(error => {
        console.log(error);
      });
    };
    handleRedirect();

  }
};

export default msalPlugin;
