import Vue from "vue"

Vue.filter("maxLength", function(value, arg) {
    if (!value) return '';
    value = value.toString();
    arg = arg*1;
    if(value.length > arg) {
        return value.substring(0, arg) + '...';
    } else {
        return value;
    }
})
