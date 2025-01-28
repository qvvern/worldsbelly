<template>
  <div class="profile-page">
    <div class="profile-page-sidebar" v-if="isSameUser">
      <div class="profile-page-sidebar__bar">
          <SideMenu />
      </div>
    </div>
    <div  class="profile-page-content-wrapper scrollbar-one" v-if="isPageReady" :style="{background: `url(${background})`, 'background-size': 'cover'}">
      <div class="profile-page-content scrollbar-one" v-if="get_user">
        <div class="profile-block">
          <div class="profile-block-left">
            <div class="profile-image">
              <div class="profile-image__main">
                <ProfileEditImageUpload :value="user.image" @onImageChange="changedImage = $event" />
              </div>
            </div>
            <div class="profile-references">
              <template  v-if="references.length > 0">
                <h4 class="p-2 pl-0"><baseIcon value="icofont-ui-social-link" :size="18" class="pr-1" color="#69bd37"/> <span class="pt-4">{{$t('socialLinks')}}</span></h4>
                <div v-for="(reference, i) in references" :key="i">
                  <div v-if="reference" class="profile-reference">
                    <div class="profile-reference-remove" v-if="$route.name == 'ProfileSettings' && isSameUser">
                      <div @click="removeReference(reference, i)">
                        <span>{{$t('remove')}}</span>
                        <baseIcon value="bi-x" :size="18" />
                      </div>
                    </div>
                    <div class="profile-reference-title pr-2">
                      <baseIcon :value="'bi-'+getHostName(reference).toLowerCase()" :size="12" class="p-1" color="#69bd37"/>
                      {{ getHostName(reference) }}
                    </div>
                    <div class="profile-reference-link px-2"><a :href="reference" target="_blank">{{getOrigin(reference)}}</a></div>
                  </div>
                </div>
              </template>
              <div v-else>
                <!-- This user has no references -->
              </div>
            </div>
          </div>
          <div class="profile-block-right pl-6 w-full">
            <div class="profile-info">
              <h1>{{ user.username }}</h1>
              <p>{{(isSameUser ? get_active_user.summary : get_user.summary ) || $t('profilePage.noProfileDescription')}}</p>
            </div>
            <div class="profile-body py-4 w-full">
              <v-tabs
                      :value="$route.name"
                      color="green"
                      slider-color="green"
                      slot="extension"
                      class="page-profile-tabs w-full"
              >
                  <v-tab
                          v-for="(tabsItem, i) in tabsItems"
                          :key="i"
                          :to="tabsItem.link"
                  >
                      <span class="pl-2 pr-2">{{ tabsItem.title }}</span>
                  </v-tab>
              </v-tabs>

                <v-container class="w-full">
                    <v-layout row class="w-full">
                        <v-flex xs12 class="w-full">
                            <v-slide-x-transition mode="out-in">
                                <router-view :user="user" class="w-full" :isSameUser.sync="isSameUser" :changedImage.sync="changedImage" :references.sync="references" @newReference="references.push($event)" />
                            </v-slide-x-transition>
                        </v-flex>
                    </v-layout>
                </v-container>
            </div>
          </div>
        </div>
      </div>
      <div class="profile-page-content scrollbar-one" v-else>
        <div class="flex justify-center w-full text-center p-5 mt-10">
            <div class="mt-10">
                <NoResults
                    :title="$t('profilePage.profileNotFound')"
                    @OnInteraction="goBack()"
                />
            </div>
        </div>
      </div>
    </div>
    <div v-else class="w-full h-full">
      <baseLoading :value="true" overlay :progressSentences="[`${$t('preparing')} ${$t('profile')}`]" />
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import _ from "lodash";
import { EventBus } from '@/event-bus'

export default {
	name: 'Administration',
	components: {
		SideMenu: () => import('@/components/administration/SideMenu.vue'),
		ProfileEditImageUpload: () => import('@/components/profile/ProfileEditImageUpload.vue'),
    NoResults: () => import('@/components/app/AppNoResults.vue'),
	},
	data() {
		return {
        isPageReady: false,
        references: [],
        user: {},
        isSameUser: false,
        changedImage: ""
		}
	},
  computed: {
      ...mapGetters("user/", ["get_active_user"]),
      ...mapGetters("user/", ["get_user"]),
        tabsItems() {
          var tabs = [
              {title: this.$t('recipes'), link: `/${this.$language.code}/profile/${this.user.username}/recipes`},
              {title: this.$t('liked'), link: `/${this.$language.code}/profile/${this.user.username}/liked`},
              {title: this.$t('reviews'), link: `/${this.$language.code}/profile/${this.user.username}/reviews`}
          ]
          if(this.isSameUser) {
            tabs.unshift({title: this.$t('settings'), link: `/${this.$language.code}/profile/${this.user.username}/settings`});
          }
          return tabs;
        },
        background() {
          return require('@/assets/images/background.png')
        }
  },
  methods: {
        ...mapActions("user/", ["fetch_user"]),
        init(user) {
          this.user = _.cloneDeep(user);
          this.changedImage = _.clone(this.user.image);
          if(this.user.references) {
            try {
              this.references = JSON.parse(this.user.references);
            }
            catch(e) {
              console.log(e);
            }
          }

          if(this.get_user?.adObjectId == this.get_active_user?.adObjectId) {
            this.isSameUser = true;
          }
          else if(this.$route.name == 'ProfileSettings') {
            this.$localeRouter.push({path: `/profile/${this.user.username}/recipes`});
          }
        },
        goBack() {
            this.$router.go(-1);
        },
        getHostName(url) {
            try {
              const hostname = url.match(/(?<![^/]\/)\b\w+\.\b\w{2,3}(?:\.\b\w{2})?(?=$|\/)/)[0];
              return hostname.split('.')[0].toUpperCase();
            } catch {
                return "";
            }
        },
        getOrigin(url) {
            if (!url) return;
            try {
                return new URL(url)?.origin;
            } catch {
                return "";
            }
        },
        removeReference(reference, i) {
          this.references.splice(i, 1);
        },
        onCancel() {
          this.init(this.get_user);
        }
  },
  created() {
      if (!EventBus._events.onCancel) {
        EventBus.$on('onCancel', this.onCancel)
      }
  },
  watch: {
    '$route.params': {
      handler() {
        if(this.$route.params?.username && this.user?.username != this.$route.params.username) {
          this.fetch_user(this.$route.params.username).finally(() => {
            this.init(this.get_user);
            this.isPageReady = true;
          }).catch(() => {
            this.isPageReady = true;
          });
        }
        else {
            this.isPageReady = true;
        }
      }, immediate: true, deep: true
    }
  }
}
</script>

<style lang="scss" scoped>
.profile-page {
  height: 100%;
  width: 100%;
  display: flex;
  &-sidebar {
    height: 100%;
    background: #FAFBFC;
    border-right: 1px solid var(--border-color);
    position: relative;
    overflow-x: hidden;
    overflow-y: auto;
    &__bar {
      height: 100%;
      width: 100%;
      display: flex;
    }
  }
  &-content-wrapper {
    position: relative;
    width: 100%;
    height: 100%;
    padding-top: 50px;
    background: #2091eb;
  }
  &-content {
    width: 100%;
    height: 100%;
    max-width: 1280px;
    background: white;
    margin: 0 auto;
    // border-top-left-radius: 25px;
    // border-top-right-radius: 25px;
    // border: 2px solid #1b1b28;
    // border-bottom: 0px;
    padding: 15px;
    -webkit-box-shadow: 0px 0px 16px 0px rgba(0,0,0,0.2) !important;
    box-shadow: 0px 0px 16px 0px rgba(0,0,0,0.2) !important;
    overflow-y: auto;
    position: relative;
    .profile-image {
      width: 275px;
      display: flex;
      &__main {
        position: relative;
        // box-shadow: #9e9e9e24 0px 0px 6px 0px;
        background: #f2f2f2a1; //#f2f2f2;
        width: 100%;
        height: 275px;
        display: flex;
        align-items: center;
        justify-content: center;
        overflow: hidden;
        flex-direction: column;
        position: relative;
      }
    }
    .profile-block {
      width: 100%;
      display: flex;
      flex-direction: row;
      // flex-wrap: wrap;
      &-left {

      }
      &-right {

      }
    }
    .profile-references {
      overflow: hidden;
      width: 275px;
      .profile-reference {
        padding: 8px 0px;
        border-top: 1px solid #e0e0e0;
        &-remove {
          display: flex;
          align-items: center;
          width: 100%;
          justify-content: flex-end;
          font-size: 11px;
          cursor: pointer;
        }
        &-link {
          font-size: 13px;
        }
        &-title {
          display: flex;
          align-items: center;
          font-size: 13px;
          font-weight: 500;
        }
      }
    }
  }
}
</style>
