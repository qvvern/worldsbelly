<template>
  <div class="admin-menu">
    <div
      v-for="(item, i) in menuItems"
      :key="i"
      class="admin-menu-item p-2"
      @click="goTo(item)"
      :class="{ active: isActive(item.routeName, item.additionalRoute),  hovered: (item.routeName == hover) }"
      @mouseover="hover = item.routeName" @mouseleave="hover = null"
    >
      <div class="admin-menu-item__line" v-if="isActive(item.routeName, item.additionalRoute)"></div>
      <div class="admin-menu-item__container">
        <v-icon class="p-1" :style="{ 'font-size': item.iconSize }">{{ item.icon }}</v-icon>
        <p>{{ item.name }}</p>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { administrationMixin } from "@/mixins/AdministrationMixin";

export default {
	name: 'SideMenu',
  mixins: [administrationMixin],
	data() {
		return {
      hover: null
		}
	},
  computed: {
      ...mapGetters("user/", ["get_active_user"]),
  },
  methods: {
    goTo(item) {
      if(item.name == 'Profile') {
        this.$localeRouter.push('/profile/'+this.get_active_user.username+'/settings');
      }
      else {
        this.$localeRouter.push({ name: item.routeName, query: this.$route.query })
      }
    }
  },
}
</script>

<style lang="scss" scoped>
.admin-menu {
    width: 100px;
    height: 100%;
    &-item {
      width: 100%;
      text-align: center;
      position: relative;
      cursor: pointer;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      &.hovered {
        background: #f1f3f7;
        transition: all 300ms;
      }
      &.active {
        background: #eeeff1;
        .v-icon {
          color: rgba(8, 8, 37, 1);
        }
      }
      &__line {
        background: #5dd223;
        position: absolute;
        height: 100%;
        width: 3px;
        left: 0;
      }
      &__container {
        .v-icon {
          color: rgba(8, 8, 37, 0.65);
          font-size: .75em;
        }
        p {
          font-size: 11px;
          font-weight: 400;
          text-align: center;
          color: #6B778C !important;
          line-height: inherit;
          transition: opacity 0.3s;
          opacity: 1;
          margin: 0;
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
        }
      }
    }
  }
</style>
