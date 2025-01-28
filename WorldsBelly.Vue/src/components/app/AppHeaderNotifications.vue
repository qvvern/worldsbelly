<template v-if="$msal.isAuthenticated">
  <div class="notification-bell" ref="notification">
    <div class="select_component" :class="{active: isOpen}" @click.prevent="isOpen = !isOpen">
        <div class="notification-amount" v-if="newNotifications.length > 0">
          <div class="notification-amount-circle">
          {{newNotifications.length}}
          </div>
        </div>
        <button class="notification-btn" :class="{'white-theme': isHome}">
          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9" />
            <path d="M13.73 21a2 2 0 0 1-3.46 0" /></svg>
        </button>
      </div>
      <ul class="select_component__options scrollbar-one" v-if="isOpen">
        <template v-if="get_notifications && get_notifications.length > 0">
          <div class="notification-actions flex w-full justify-end px-3 py-1">
              <BaseUiButton
                  @click.prevent.native="deleteAll"
                  :size="11"
                  type="btn3"
                  style="height: 28px; min-width: 100px"
                  class="ml-2"
                  :disabled="get_notifications.length == 0"
              >
                  {{ $t('deleteAll') }}
              </BaseUiButton>
              <BaseUiButton
                  @click.prevent.native="readAll"
                  :size="11"
                  type="btn2"
                  style="height: 28px; min-width: 100px"
                  class="ml-2"
                  :disabled="newNotifications.length == 0"
              >
                  {{ $t('readAll') }} ({{newNotifications.length}})
              </BaseUiButton>
          </div>
          <li
            class="notification-message"
            v-for="notification in get_notifications"
            :key="notification.id"
            @click="readMessage(notification)">
                <!-- <p class="notification-message-title" v-html="notification.title"></p> -->
                <p class="notification-message-read flex justify-end" style="font-size: 11px" v-if="notification.isRead" :class="{isread: notification.isRead}">
                  Read&nbsp;<b>{{ getDate(notification.readAt) }}</b>
                </p>
                <p class="notification-message-read flex justify-end" style="font-size: 11px" v-else :class="{isread: notification.isRead}">
                  Recieved&nbsp;<b>{{ getDate(notification.createdAt) }}</b>
                </p>
                <p class="notification-message-content pb-2" v-html="notification.content" :class="{isread: notification.isRead}"></p>
            </li>
            <BaseIntersectionObserver v-if="get_notifications && get_notifications.length > 1 && !noMore" @intersecting="intersecting($event)" />
        </template>
        <template v-else>
          <div class="notification-actions flex w-full justify-center px-3 py-1" style="min-width: 180px; cursor: auto">
            <p style="font-size: 13px">{{ $t('noNotifications') }}</p>
          </div>
        </template>
      </ul>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
    export default {
        name: "AppHeaderNotifications",
        props: {
          isHome: Boolean
        },
        data: () => ({
            connection: null,
            isOpen: false,
            loading: true,
            loadingMore: false,
            noMore: false
        }),
        computed: {
          ...mapGetters("notification/", ["get_notifications"]),
          msalUser() {
              return this.$msal.getProfile()?.idTokenClaims;
          },
          newNotifications() {
            return this.get_notifications?.filter(_ => !_.isRead) ?? [];
          }
        },
        methods: {
          ...mapActions("notification/", ["fetch_notifications", "fetch_append_notifications", "fetch_notification", "delete_notifications", "read_notification", "read_notifications"]),
          getDate(readAt) {
            return this.$moment.getTimeSinceText(readAt);
          },
          setupConnection() {
              this.connection = this.$connection.build();

              this.connection.on("Notification", data => {
                this.fetch_notification(data.id);
              });

              this.connection
                  .start()
                  .then(() => {
                      this.subscribeUser();
                  })
                  .catch(function(e) {});
          },
          subscribeUser() {
              if (this.msalUser?.oid == null) {
                  console.warn("Could not subscribe user");
                  return;
              }
              this.connection.invoke("SubscribeToNotifications", this.msalUser.oid).catch(function(err) {
                  return console.error(err.toString());
              });
          },
          unsubscribeUser() {
              if (this.msalUser?.oid == null) {
                  console.warn("Could not unsubscribe user");
                  return;
              }

              this.connection.invoke("UnsubscribeToNotifications", this.msalUser.oid).catch(function(err) {
                  return console.error(err.toString());
              });
          },
          close() {
            this.isOpen = false;
          },
          documentClick(e) {
            if (this.isOpen) {
              const el = this.$refs["notification"];
              const { target } = e;
              if (el !== target && !el.contains(target)) {
                this.isOpen = false;
              }
            }
          },
          readMessage(notification) {
            if(notification.isRead) return;
            this.read_notification(notification.id);
            this.get_notifications.find(_ => _.id == notification.id).isRead = true;
          },
          readAll() {
            this.read_notifications();
            this.get_notifications.forEach(n => {
              this.get_notifications.find(_ => _.id == n.id).isRead = true;
            })
            this.close();
          },
          deleteAll() {
            this.delete_notifications();
            this.close();
          },
          intersecting(intersecting) {
            if (
                !this.loading &&
                !this.loadingMore &&
                intersecting &&
                this.get_notifications &&
                !this.noMore &&
                this.get_notifications.length >= 50
            ) {
                if(!this.noMore) {
                  this.loadingMore = true;
                  this.fetch_append_notifications().finally((response) => {
                    if(response < 50) {
                      this.noMore = true;
                    }
                    this.loadingMore = false;
                  })
                }
            }
            return;
          }
        },
        beforeDestroy() {
            this.unsubscribeUser();
        },
        destroyed() {
          document.removeEventListener("mousedown", this.documentClick);
        },
        created() {
          this.loading = true;
          this.fetch_notifications().finally(() => {
            this.loading = false;
          });
          document.addEventListener("mousedown", this.documentClick);
          if (this.connection) {
              this.unsubscribeUser();
          }
          this.setupConnection();
        }
    };
</script>

<style lang="scss" scoped>
@media only screen and (max-width: 800px) {
  .notification-bell {
   margin-top: 0px !important;
  }
}
.notification-bell {
  cursor: pointer;
  position: relative;
  margin-top: 4px;
  .notification-amount {
    position: absolute;
    top: -10px;
    right: -5px;
    &-circle {
      background: #FF5A00;
      border-radius: 3em;
      -moz-border-radius: 3em;
      -webkit-border-radius: 3em;
      color: #ffffff;
      height: 1.8em;
      width: 1.8em;
      display: inline-block;
      font-family: 'Roboto', Helvetica, Sans-serif;
      font-size: 11px;
      line-height: 2em;
      text-align: center;
    }
  }
  .notification-btn {
    color: #1f1c2e;
    padding: 0;
    border: 0;
    background-color: transparent;
    height: 32px;
    display: flex;
    justify-content: center;
    align-items: center;
    &.white-theme {
      color: #fff;
    }
  }

  .select_component {
    position: relative;
    &:hover, &.active {
    }
    &__selected {
    }
    &__options {
      max-height: 500px;
      margin-left: 2px;
      margin-top: -6px;
      border-bottom-left-radius: 8px;
      border-bottom-right-radius: 8px;
      position: absolute;
      z-index: 999999;
      right: -50%;
      list-style-type: none;
      padding-left: 0px;
      background-color: #fff;
      -webkit-box-shadow: 0 0 10px rgba(0,0,0,0.2);
      -moz-box-shadow: 0 0 10px rgba(0,0,0,0.2);
      box-shadow: 0 0 10px rgba(0,0,0,0.2);
      overflow-y: auto;
      .notification-actions {
        background: #eeeeee;
        position: sticky;
        z-index: 9;
        top: 0;
      }
      li {
        width: 300px;
        font-size: 13px;
        padding: 6px 8px;
        border-bottom: 1px solid #ccc;
        .isread {
          opacity: .4;
        }
        &:last-child {
          border-bottom: 0px;
        }
        cursor: auto;
        &:hover {
          background-color: #fff;
        }
        &.selected {
          background-color: rgb(196, 196, 196);
        }
      }
    }

  }
}
</style>
