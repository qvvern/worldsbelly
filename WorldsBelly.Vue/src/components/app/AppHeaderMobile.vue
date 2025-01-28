<template>
    <div class="app-bar-mb flex justify-between items-center" v-if="!isLoginPage">
        <div class="app-bar-mb-logo" @click="$localeRouter.push('/home')">
            <img :src="logo" alt="Logo for worlds belly" />
        </div>
        <!-- <div class="app-bar-mb-search" v-if="!isHome">
        </div> -->
        <div class="app-bar-mb-actions flex flex-auto items-center justify-end">
        <LanguageSelector :isHome="isHome" />
            <BaseSearchInputMobile
                v-if="!isHome"
                :isHome="isHome"
                style="margin-top: 4px"
                class="ml-3"
                :placeholder="$t('searchPlaceholder')"
                small
                outlined
            />
            <v-divider
                class="mx-3"
                style="height: 25px; margin-top: 4px"
                vertical
                :style="{ 'border-color': !isHome ? null : 'rgba(255,255,255,.6)' }"
            />
            <div class="mr-3 ml-2" v-if="!$msal.isAuthenticated && !isLoginPage">
                <v-btn
                  icon
                  small
                  @click.prevent.native="signIn"
                  v-on="on"
                >
                  <v-avatar
                    color="#f7f7f7"
                    size="33"
                    style="border: 1px solid #d6d6d6"
                  >
                    <baseIcon class="pt-2" :size="26" color="#253a54" value="icofont-user" />
                  </v-avatar>
                </v-btn>
            </div>
            <AppHeaderNotifications :isHome="isHome" class="mr-5" v-if="$msal.isAuthenticated"/>
            <AppHeaderProfile class="mr-4" v-if="$msal.isAuthenticated" />
        </div>
    </div>
</template>

<script>
import { mapGetters } from "vuex";
import { EventBus } from '@/event-bus'

export default {
    name: "appHeader",
    props: {
        isHome: Boolean,
    },
    data: () => {
        return {
            searchText: null,
        };
    },
    components: {
        AppHeaderProfile: () => import("@/components/app/AppHeaderProfile.vue"),
        AppHeaderNotifications: () => import("@/components/app/AppHeaderNotifications.vue"),
        LanguageSelector: () => import("@/components/base/LanguageSelector.vue"),
    },
    computed: {
        ...mapGetters("recipe/", ["get_recipes_filter"]),
        isLoginPage() {
          return this.$route.name == 'Login';
        },
        logo() {
            return require("@/assets/images/logo-mobile.svg");
        },
    },
    methods: {
        signIn() {
            this.$router.push({ name: "Login", query: { redirect: this.$route.path } });
        },
        search() {
          EventBus.$emit("onSearch", this.searchText);
        }
    },
    watch: {
      'get_recipes_filter.search': {
        handler(search) {
          if(this.searchText != search) {
            this.searchText = search;
          }
        }, deep: true, immediate: true
      }
    }
};
</script>

<style lang="scss" scoped>
.app-bar-mb {
    position: relative;
    width: 100%;
    height: 45px;
    z-index: 9;
    background: transparent !important;
    transition: all 200ms;
    box-shadow: 0px 0px 3px 2px rgb(0 0 0 / 5%);
    // border-bottom: 1px solid var(--border-color);
    &-logo {
        opacity: 1;
        height: 20px;
        display: flex;
        align-items: center;
        padding-left: 10px;
        margin-right: 3px;
        img {
            height: 100%;
        }
    }
    &-search {
        width: 100%;
        display: flex;
    }
    &-actions {
        // min-width: 200px;
    }
}
</style>
