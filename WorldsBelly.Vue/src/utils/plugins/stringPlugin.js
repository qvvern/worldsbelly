/* eslint-disable */
const stringPlugin = {
  install(Vue) {
    Vue.prototype.$string = {};

    Vue.prototype.$string.maxLength = function (value, length) {
      if (!value) return '';
      value = value.toString();
      length = length*1;
      if(value.length > length) {
          return value.substring(0, length) + '...';
      } else {
          return value;
      }
    };
  }
};

export default stringPlugin;
