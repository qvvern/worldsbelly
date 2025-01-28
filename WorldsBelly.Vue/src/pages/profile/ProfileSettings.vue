<template>
    <div class="page-profile-settings w-full" v-if="isSameUser">
      <ProfileDeleteDialog
        ref="confirmDelete"
        @confirm="deleteProfile()"
        :title="$t('profilePage.deleteAccountTitle')"
        width="600px"
        :description="$t('profilePage.deleteAccountText')"
        :confirmBtn="$t('profilePage.deleteConfirmBtn')"
        :cancelBtn="$t('profilePage.deleteCancelBtn')"
        />
        <div class="w-full flex flex-col justify-between" style="max-width: 500px">
            <div class="w-full flex flex-wrap pt-6">
              <ProfileEditUsername v-model="userSettings.username" :disabled="isSaving" />
            </div>
            <div class="w-full flex flex-wrap">
                <v-textarea
                    v-model="userSettings.summary"
                    maxlength="255"
                    :label="$t('description')"
                    outlined
                    :disabled="isSaving"
                    rows="4"
                />
            </div>
            <div class="w-full flex flex-col">
              <p class="pb-2" style="font-size: 12px">{{ $t('profilePage.addReference') }}</p>
              <div class="w-full flex">
                <v-text-field
                    dense
                    :label="$t('link')"
                    outlined
                    v-model="newReference"
                    :disabled="isSaving"
                    :rules="[rules.validUrl]"
                />
                <baseButton :disabled="!newReference || !isValidHttpUrl(newReference)" tertiary style="height: 40px" @click.prevent.native="addReference()" class="ml-2" noshadow>{{ $t('add') }}</baseButton>
              </div>
            </div>
        </div>
        <div class="w-full flex flex-col" style="max-width: 500px">
            <div class="page-profile-settings__title flex justify-between">{{ $t('profilePage.privateSettings') }}</div>
            <div class="w-full flex flex-col pt-8">
                <div class="w-full flex flex-wrap">
                    <v-text-field
                        dense
                        v-model="msalSettings.givenName"
                        :rules="[rules.required, rules.minLength2]"
                        :label="$t('firstName')"
                        class="mr-4"
                        :disabled="isSaving"
                    />
                    <v-text-field dense :rules="[rules.required, rules.minLength2]" v-model="msalSettings.surname" :label="$t('lastName')" :disabled="isSaving" />
                </div>
                <div class="w-full flex flex-wrap pt-4">
                    <h4 class="w-full">{{$t('email')}}</h4>
                    <template v-if="!provider">
                      <p class="mr-4">{{ email }}</p>
                      <button class="change-email-btn" @click="changeEmail = !changeEmail">({{$t('change')}} {{$t('email')}})</button>
                    </template>
                    <span v-else style="font-size: 12px">{{ $t('noEmailRequired') }}. ({{provider}})</span>
                </div>
                <div class="w-full flex flex-col pt-6" v-if="changeEmail && !provider">
                    <v-text-field dense v-model="emailSettings.currentMail" :label="$t('typeYourCurrentEmail')" :disabled="isSaving" :rules="[rules.required, rules.sameMail]" />
                    <div class="w-full flex flex-wrap pt-2">
                      <v-text-field dense v-model="emailSettings.newMail" :label="$t('typeYourNewEmail')" :disabled="isSaving" class="mr-2" :rules="[rules.required, rules.validMail, rules.notSameMail, rules.confirmMail]" />
                      <v-text-field dense v-model="emailSettings.newMailConfirm" :label="$t('confirmNewEmail')" :disabled="isSaving" :rules="[rules.required, rules.validMail, rules.notSameMail, rules.confirmMail]" />
                    </div>
                    <baseButton tertiary style="height: 40px" :disabled="!isMailSettingsValid || isSending" :loading="isSending" @click.prevent.native="sendVerificationCode()" class="mt-2" noshadow>{{$t('send')}} {{$t('verificationCode')}}</baseButton>
                    <template v-if="confirmEmailStatus">
                      <p style="font-size: 12px;color: red;">{{confirmEmailStatus}}</p>
                      <v-text-field class="my-6" dense v-model="msalSettings.verificationCode" :label="$t('verificationCode')" :disabled="isSaving" :rules="[rules.required]" />
                    </template>
                </div>
                <div class="w-full flex flex-wrap pt-6" :class="{'is-not-editable': isMsalSettingsChanged || isUserSettingsChanged}">
                    <h4 class="w-full">Password</h4>
                    <button class="change-email-btn" @click="resetPassword" v-if="!provider">({{$t('change')}} {{$t('password')}})</button>
                    <span v-else style="font-size: 12px">{{$t('noPasswordRequired')}}. ({{provider}})</span>
                </div>
                <div class="w-full flex flex-wrap pt-6" :class="{'is-not-editable': isMsalSettingsChanged || isUserSettingsChanged}">
                    <h4 class="w-full">{{$t('delete')}} {{$t('profile')}}</h4>
                    <button class="change-email-btn" @click="$refs.confirmDelete.open()">{{$t('clickHereIfYouWishToDeleteYourAccount')}}.</button>
                </div>
            </div>
        </div>
        <ProfileEditActions
            v-if="isUserSettingsChanged || isMsalSettingsChanged"
            :user.sync="userSettings"
            :msalUser.sync="msalSettings"
            :isUserSettingsChanged="isUserSettingsChanged"
            :isMsalSettingsChanged="isMsalSettingsChanged"
            @resetProfile="setUserSettings()"
            @isSaving="updateChanges($event)"
            @isSavingError="isSaving = $event"
        />
    </div>
</template>

<script>
import _ from "lodash";
import { EventBus } from "@/event-bus";
import { mapActions } from 'vuex'
export default {
    props: {
        user: Object,
        changedImage: String,
        references: Array,
        isSameUser: Boolean
    },
    components: {
        ProfileEditUsername: () => import("@/components/profile/ProfileEditUsername.vue"),
        ProfileEditActions: () => import("@/components/profile/ProfileEditActions.vue"),
        ProfileDeleteDialog: () => import("@/components/profile/ProfileDeleteDialog.vue"),
    },
    data() {
        return {
          newReference: null,
          rules: {
            required: v => !!v || 'Field is Required.',
            sameMail: v => v == this.email || 'Mail doesnt match your current mail',
            confirmMail: v => v == this.emailSettings.newMail || 'Mail doesnt match your current mail',
            validMail: v => v?.includes('@') || 'its not a valid email.',
            notSameMail: v => v != this.email || 'Mail cannot be the same as your current mail.',
            minLength2: v => v?.length >= 2 || 'This field must have atleast 2 characters.',
            validUrl: v => !v || this.isValidHttpUrl(v) || 'Url is invalid.',
          },
            userSettings: {},
            msalSettings: {},
            isUserSettingsChanged: false,
            isMsalSettingsChanged: false,
            isSaving: false,
            changeEmail: false,
            emailSettings: {
              currentMail: null,
              newMail: null,
              newMailConfirm: null
            },
            confirmEmailStatus: null,
            isSending: false
        };
    },
    computed: {
      isMailSettingsValid() {
        if(this.emailSettings.currentMail == null ||
        this.emailSettings.currentMail != this.email ||
        this.emailSettings.newMail == null ||
        this.emailSettings.newMail != this.emailSettings.newMailConfirm ||
        this.emailSettings.email == this.emailSettings.newMail ||
        !this.emailSettings.newMail?.includes('@')) {
          return false;
        }
        return true
      },
        msalUser() {
            return this.$msal.getProfile()?.idTokenClaims;
        },
        familyName() {
            return this.msalUser?.family_name;
        },
        givenName() {
            return this.msalUser?.given_name;
        },
        email() {
            return this.msalUser?.emails[0];
        },
        provider() {
            return this.msalUser?.idp;
        },
    },
    methods: {
        ...mapActions('user/', ['send_email_verification_code']),
        addReference() {
          if(this.isValidHttpUrl(this.newReference)) {
            const newUrl = this.newReference;
            this.$emit('newReference', newUrl);
            this.newReference = null;
          }
        },
        isValidHttpUrl(string) {
          let url;
          try {
            url = new URL(string);
            const hostname = url.href.match(/(?<![^/]\/)\b\w+\.\b\w{2,3}(?:\.\b\w{2})?(?=$|\/)/)[0];
            return hostname ? (url.protocol === "http:" || url.protocol === "https:") : false;
          } catch (_) {
            return false;
          }
        },
        resetPassword () {
          window.location = "https://worldsbelly.b2clogin.com/worldsbelly.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_reset_password&client_id=9db55ba2-55dc-4ec7-b05f-368417756ee8&nonce=defaultNonce&redirect_uri=http%3A%2F%2Flocalhost%3A8080%2Flogin&scope=openid&response_type=id_token&prompt=login";
        },
        sendVerificationCode() {
          this.isSending = true;
          if(this.isMailSettingsValid) {
            this.send_email_verification_code({
              oldEmail: this.emailSettings.currentMail,
              newEmail: this.emailSettings.newMail
            }).then(() => {
              this.confirmEmailStatus = "Email verification sent. Please check your spam folder. Expires in 30 minutes."
            }).catch((e) => {
              this.confirmEmailStatus = e
            }).finally(() => {
              this.msalSettings.mail = this.emailSettings.newMail;
              this.isSending = false;
            })
          }
        },
        setUserSettings() {
            this.userSettings = _.cloneDeep(this.user);
            this.msalSettings = {
                surname: this.familyName,
                givenName: this.givenName,
                verificationCode: null
            };
            EventBus.$emit("resetProfileImage", this.userSettings.image);
            EventBus.$emit("onCancel");

        },
        changeImage(image) {
            this.userSettings.image = _.clone(image);
        },
        updateChanges(isSaving) {
          this.isSaving = isSaving;
          if(!isSaving) {
            this.isMsalSettingsChanged = false;
            this.isUserSettingsChanged = false;
          }
        }
    },
    destroyed() {
        this.setUserSettings();
    },
    created() {
        this.setUserSettings();
    },
    watch: {
        changedImage: {
            handler() {
                this.changeImage(this.changedImage);
            },
        },
        userSettings: {
            handler() {
                if (_.isEqual(this.userSettings, this.user)) {
                    this.isUserSettingsChanged = false;
                } else {
                    this.isUserSettingsChanged = true;
                }
            },
            deep: true,
        },
        msalSettings: {
            handler() {
                if (this.msalSettings.surname == this.familyName && this.msalSettings.givenName == this.givenName && this.msalSettings.verificationCode == null) {
                    this.isMsalSettingsChanged = false;
                } else {
                    this.isMsalSettingsChanged = true;
                }
            },
            deep: true,
        },
        references: {
          handler() {
            this.userSettings.references = JSON.stringify(this.references)
          }, deep: true
        }
    },
};
</script>

<style lang="scss" scoped>
.page-profile-settings {
  h4 {
    font-size: 13px;
  }
  .is-not-editable {
    opacity: .5;
    pointer-events: none;
  }
  &__title {
    font-weight: 500;
    font-size: 22px;
  }
  .change-email-btn {
    font-size: 12px;
  }
}
</style>
