<template v-if="$msal.isAuthenticated">
    <div v-if="profile">
        <v-toolbar dense color="darkblue" dark extended extension-height="400" flat>
            <v-toolbar-title class="mx-auto text-venter" slot="extension">
                <h2>Thank you for signing up!</h2>
                <span>We have already given you a username. Change your username if you like..</span>
                <div class="flex mt-4">
                  <ProfileEditUsername v-model="profile.username" @isValid="isValid = $event"/>
                  <baseButton
                    tertiary
                    light
                    style="height: 50px; padding: 0px 30px !important;"
                    class="ml-4 mt-1"
                    :disabled="!isValid"
                    @click.prevent.native="updateUser()"
                    :loading="isUpdatingUsername"
                  >
                    Update
                  </baseButton>
                </div>
            </v-toolbar-title>
        </v-toolbar>
          <div class="w-full flex flex-wrap p-3 justify-center">
            <div class="drawing">
              <div class="chef-hat">
                <div class="chef-hat__ruffle"></div>
                <div class="chef-hat__ruffle"></div>
                <div class="chef-hat__ruffle"></div>
              </div>
            </div>
              <div class="flex w-full flex-col flex-wrap p-3 justify-center items-center">
                    <baseButton thick @click.prevent.native="redirectHandler()">Continue</baseButton>
                    <h4 class="pb-2 my-2">Continue to where you left off after sign up.</h4>
              </div>
          </div>
    </div>
  <baseProgressLinear v-else />
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import _ from "lodash";

export default {
    name: "SetUpProfile",
    components: {
        ProfileEditUsername: () => import("@/components/profile/ProfileEditUsername.vue"),
    },
    data() {
        return {
          profile: null,
          isValid: false,
          errors: {},
          isUpdatingUsername: false
        };
    },
    computed: {
        ...mapGetters("user/", ["get_active_user"]),
    },
    methods: {
        ...mapActions("user/", ["validate_username", "update_active_username"]),
        redirectHandler() {
          if(this.$route.query.redirect && this.$route.query.redirect != "/SetUpProfile") {
            this.$localeRouter.push(this.$route.query.redirect);
          }
          else {
            this.$localeRouter.push("/");
          }
        },
        updateUser() {
          if(this.isValid && !this.isUpdatingUsername) {
            this.isUpdatingUsername = true;
            this.update_active_username(this.profile.username).then(() => {
                this.redirectHandler();
                this.isUpdatingUsername = false;
            })
          }
        }
    },
    mounted() {
        const newUser =  this.$msal.getProfile()?.idTokenClaims?.newUser;
        if(!newUser) this.$localeRouter.push("/");
        else this.profile = _.cloneDeep(this.get_active_user);
    },
};
</script>

<style lang="scss" scoped>
.drawing {

  .chef-hat {
    position: relative;
    background: #EFEFEF;
    height: 60px;
    width: 100px;
    box-shadow: -8px 0 0 0 #AAA inset, -48px 0 0 0 #CCC inset;
    border-radius: 0 0 8px 8px;
    // transform: rotate(-22.5deg);
  }

  $i: 0;
  $ruffles: ((60px, 60px, 0, -20px, translate(0, -75%)), (80px, 80px, 0, 50%, translate(-50%, -75%)), (60px, 60px, 0, 100%, translate(-40px, -75%)));
  @each $height,
  $width,
  $x,
  $y,
  $transform in $ruffles {
    $i: $i + 1;
    .chef-hat__ruffle:nth-child(#{$i}) {
      position: absolute;
      height: $height;
      width: $width;
      background: #EFEFEF;
      top: $x;
      left: $y;
      transform: $transform;
      border-radius: 60px;
      box-shadow: -4px -8px 0 0 #AAA inset, -8px -16px 0 0 #CCC inset;
    }
  }

}
</style>
<style lang="scss">
</style>
