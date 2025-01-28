<template>
      <div
          style="text-align: right;"
      >
          <p style="font-size: 12px; color: rgba(0, 0, 0, 0.6)">{{ title }}</p>
          <div class="flex items-center justify-end">
                <div v-if="flag" style="width: 40px;position: relative;height: 20px;">
                  <country-flag size="small" style="position: absolute;top: -10px;margin: 0px;left: 0" shadow :country='flag' />
                </div>
              <p style="font-size: 13px">
                  {{ text }}
                  <slot></slot>
              </p>
          </div>
      </div>
</template>

<script>
export default {
    name: "RecipeTopBox",
    props: {
        title: [Number, String],
        text: [Number, String],
        flag: [Number, String],
    },
    components: {
      CountryFlag: () => import('vue-country-flag'),
    },
};
</script>
