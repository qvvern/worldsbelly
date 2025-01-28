import _ from "lodash"
export const QueryMixin = {
  name: "QueryMixin",
  methods: {
    isBool(string) {
      if(string == "false" || string == "true") return true;
      return false
    },
    updateQueryLocal() {
      let queries = JSON.parse(JSON.stringify(this.$route.query));
      for (const [key, value] of Object.entries(queries)) {
        if(key == 'time') {
          var tList = queries[key].split(':');
          this.queries[key] = {
            amount: Number(tList[0]),
            option: Number(tList[1])
          }
        }
        else if(key == 'nutrients') {
          this.queries[key] = queries[key].split(";").map(_ => {
            var nList = _.split(':');
            return {
              id: nList[0],
              amount: Number(nList[1]),
              option:  Number(nList[2])
            }
          });
        }
        else if(Array.isArray(this.queries[key])) {
          this.queries[key] = queries[key].split(";").map(x => Number(x));
        }
        else {
          this.queries[key] = isNaN(queries[key]) ?
            this.isBool(queries[key]) ? Boolean(queries[key]) : queries[key] : Number(queries[key]);
        }
      }
    },
    addQuery: _.debounce(function() {
      this.addParamsToLocation(this.queries);
    }, 300),
    addParamsToLocation(params) {
      history.replaceState({}, null, `${this.$route.path+this.buildParams(params)}`)
    },
    buildParams(filter) {
      var params = Object.keys(filter).map((key) => {
        if(key == 'time') {
          if(!_.isEmpty(filter[key])) {
            return `${key}=${filter[key].amount}:${filter[key].option}`;
          }
          return;
        }
        else if(key == 'nutrients') {
          if(filter[key]?.length > 0) {
            var nutrients = filter[key].map(_ => {
              return `${_.id}:${_.amount}:${_.option}`;
            });
            return  `${key}=${nutrients.join(";")}`;
          }
          return;
        }
        else if(Array.isArray(filter[key])) {
          if(filter[key]?.length > 0) {
            return  `${key}=${filter[key].join(";")}`;
          }
          return;
        }
        else if(filter[key]) {
          return  `${key}=${filter[key].toString()}`;
        }
        return;
      });
      params = params.filter(_ => _ != null && _ != undefined && _ != "").join('&');
      if(params) {
        if(this.$route.name != 'Recipes') {
          var query = {};
          var arr = params?.split('&');
          for (let index = 0; index < arr.length; index++) {
            try {
              var item = arr[index]?.split('=');
              query[item[0]] = item[1];
            }
            catch(e){
              continue;
            }
          }
          this.$localeRouter.push({
              name: "Recipes",
              query: query,
          });
        }
        else {
          return '?'+params;
        }
      }
      return "";
    },
  },
  mounted() {
    this.updateQueryLocal();
    this.addQuery();
    this.$router.beforeEach((to, from, next) => {
      if(to.path == from.path) return;
      next();
    });
  },
  watch: {
    queries: {
      handler() {
        this.addQuery();
      },
      deep: true,
    },
  },
};
