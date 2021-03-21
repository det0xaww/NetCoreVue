<template>
    <div class="home">
        <img height="200" width="500" alt="Vue logo" src="../assets/nbaTeamsLogos.jpg">
        <HelloWorld msg="NBA teams" />
    </div>
</template>

<script>
// @ is an alias to /src
import HelloWorld from '@/components/HelloWorld.vue'


export default {
  name: 'Home',
  components: {
    HelloWorld
  }
}
</script>