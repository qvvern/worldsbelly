/* eslint-disable */
import router from '@/router'

const localeRouterPlugin = {
  install(Vue) {
    Vue.prototype.$localeRouter = Vue.observable({
      push: push,
    });

    function push(e) {
      let props = router.resolve(e);
      const langCode = Vue.prototype.$language?.code;
      if(langCode) {
        const firstPath = props.href?.split("/")?.[1] ?? '';
        if(firstPath == langCode) {
          router.push(e)
        }
        else {
          router.push({path: "/"+langCode+props.href})
        }
      }
      else {
        router.push(e)
      }
    };
  }
};

export default localeRouterPlugin;
