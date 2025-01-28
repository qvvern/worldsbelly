<template v-if="$msal.isAuthenticated">
<div>
      <v-menu
        bottom
        min-width="200px"
        rounded
        offset-y
      >
        <template v-slot:activator="{ on }">
          <v-btn
            icon
            small
            v-on="on"
          >
            <v-avatar
              color="#f7f7f7"
              size="33"
              style="border: 1px solid #d6d6d6"
            >
              <img v-if="get_active_user.image" :src="'https://worldsbellystorage.blob.core.windows.net'+get_active_user.image" />
              <baseIcon v-else class="pt-2" :size="26" color="#253a54" value="icofont-user" />
            </v-avatar>
          </v-btn>
        </template>
        <v-card>
          <v-list dense>
              <v-list-item @click="goTo('Profile')">
                <v-list-item-icon>
                  <baseIcon :size="16" color="#253a54" value="icofont-user" />
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title>{{ $t('profile') }}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
              <v-list-item @click="goTo('RecipePublished')">
                <v-list-item-icon>
                  <baseIcon :size="16" color="#253a54" value="icofont-food-basket" />
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title>{{ $t('recipes') }}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
              <v-list-item @click="signOut">
                <v-list-item-icon>
                  <baseIcon :size="16" color="#253a54" value="icofont-user" />
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title>{{ $t('signOut') }}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
          </v-list>
        </v-card>
      </v-menu>
</div>
</template>

<script>
import { mapGetters } from "vuex";
    export default {
        name: "AppHeaderProfile",
        data: () => ({
        }),
        computed: {
      ...mapGetters("user/", ["get_active_user"]),
          logo() {
            return require('@/assets/images/logo.svg')
          }
        },
        methods: {
          goTo(routeName) {
            if(routeName == 'Profile') {
              this.$localeRouter.push('/profile/'+this.get_active_user.username+'/settings');
            }
            else {
              this.$localeRouter.push({ name: routeName });
            }
          },
          signOut() {
            this.$localeRouter.push("/");
            this.$msal.signOut();
          }
        }
    };
</script>

<style lang="scss" scoped>
</style>
