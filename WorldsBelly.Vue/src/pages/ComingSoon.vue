<template>
<div class="bgimg">
  <div class="topleft">
    <img :src="logo" alt="Logo for worlds belly" style="width: 150px;"/>
  </div>
  <div class="middle">
    <h1>COMING SOON</h1>
    <hr>
    <p>Website is coming soon. We are working on the full version of our new site and will be back around July 2023.</p>
  </div>
</div>
</template>


<script>
export default {
    name: "ComingSoon",
    computed: {
        logo() {
            return require("@/assets/images/logo.svg");
        },
    },
}
</script>

<style lang="scss" scoped>
.bgimg {
  background-image: url('https://worldsbellystorage.blob.core.windows.net/b2c/background.jpg');
  height: 100%;
  background-position: center;
  background-size: cover;
  position: relative;
  color: white;
  font-family: "Open Sans", Courier, monospace;
  font-size: 25px;
}

.topleft {
  position: absolute;
  top: 22px;
  left: 22px;
}

.bottomleft {
  position: absolute;
  bottom: 0;
  left: 16px;
}

.middle {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  text-align: center;
}
p {
    font-size: 18px
}
h1 {
    font-weight: 800;
}
svg {
    vertical-align: middle;
}
hr {
  margin: auto;
  width: 40%;
}
</style>
