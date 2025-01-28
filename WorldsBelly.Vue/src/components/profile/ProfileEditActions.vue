<template>
  <div class="profile-edit-actions p-3 w-full">
    <div class="profile-edit-actions__options">
      <p style="font-size: 12px;color: red;" class="my-4" v-if="globalError">{{globalError}}</p>
      <baseButton v-if="!isSaving" size="14" tertiary  delete @click.prevent.native="$emit('resetProfile')" class="mr-5">Cancel</baseButton>
      <baseButton
        size="14"
        tertiary
        black
        @click.prevent.native="saveProfile()"
        :loading="isSaving"
        :disabled="isSaving"
      >
        {{ isSaving ? 'Saving' : 'Save changes' }}
      </baseButton>
    </div>
  </div>
</template>

<script>
import { mapActions } from 'vuex'
  export default {
      name: "ProfileEditActions",
      props: {
        user: Object,
        msalUser: Object,
        isUserSettingsChanged: Boolean,
        isMsalSettingsChanged: Boolean,
      },
      data() {
        return {
          isSaving: false,
          isDeleting: false,
          globalError: null
        }
      },
      methods: {
        ...mapActions('user/', ['update_active_user', 'update_active_msalUser']),
        saveProfile() {
          this.globalError = null;
          this.isSaving = true;
          this.$emit('isSaving', true);
          if(this.isUserSettingsChanged && this.isMsalSettingsChanged) {
            this.update_active_user(this.user).then(() => {
              this.update_active_msalUser(this.msalUser).then(() => {
                this.isSaving = false;
                this.$emit('isSaving', false);
              }).catch((e) => {
                this.isSaving = false;
                this.$emit('isSavingError', false);
                if(e) this.globalError = e;
            })
            }).catch((e) => {
                this.isSaving = false;
                this.$emit('isSavingError', false);
                if(e) this.globalError = e;
            })
          }
          else if(this.isUserSettingsChanged) {
            this.update_active_user(this.user).then(() => {
                this.isSaving = false;
                this.$emit('isSaving', false);
                this.$localeRouter.push('/profile/'+this.user.username+'/settings');
            }).catch((e) => {
                this.isSaving = false;
                this.$emit('isSavingError', false);
                if(e) this.globalError = e;
            })
          }
          else if(this.isMsalSettingsChanged) {
            this.update_active_msalUser(this.msalUser).then(() => {
                this.isSaving = false;
                this.$emit('isSaving', false);
            }).catch((e) => {
                this.isSaving = false;
                this.$emit('isSavingError', false);
                if(e) this.globalError = e;
            })
          }
          else {
              this.isSaving = false;
              this.$emit('isSaving', false);
          }
        }
      },
      mounted() {
      }
  };
</script>

<style lang="scss" scoped>
.profile-edit-actions {
  position: sticky;
  width: 100%;
  z-index: 9999;
  right: 0px;
  top: 0px;
  bottom: 0px;
  display: flex;
  align-items: center;
  justify-content: flex-end;
  &__options {
    border: 1px solid #fff;
    background: #fff;
    border-radius: 10px;
    box-shadow: 0px 0px 10px -1px rgba(0, 0, 0, 0.2);
    padding: 10px 20px;

  }
}
</style>
