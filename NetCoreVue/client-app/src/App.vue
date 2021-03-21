<template>
    <div id="app">
        <notifications position="top right" group="foo" />
        <!--<div id="nav">
            <router-link to="/">Home</router-link> |
            <router-link to="/teams">Teams</router-link>
        </div>-->
        <b-navbar type="light" variant="light">
            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
            <b-navbar-brand to="/">Home</b-navbar-brand>
            <b-collapse is-nav id="nav-collapse">
                <b-navbar-nav>
                    <b-nav-item to="/teams">Teams</b-nav-item>
                    <b-nav-item href="#" @click.prevent="login" v-if="!user">Login</b-nav-item>
                    <b-nav-item href="#" @click.prevent="logout" v-else>Logout</b-nav-item>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
        <router-view />
    </div>
</template>

<script>

    export default {
        name: 'app',
        data() {
            return {
                user: null
            }
        },
        async created() {
            await this.refreshUser()
        },
        watch: {
            '$route': 'onRouteChange'
        },
        methods: {
            login() {
                this.$auth.loginRedirect()
            },
            async onRouteChange() {
                // every time a route is changed refresh the user details
                await this.refreshUser()
            },
            async refreshUser() {
                // get new user details and store it to user object
                this.user = await this.$auth.getUser()
            },
            async logout() {
                await this.$auth.logout()
                await this.refreshUser()
                this.$router.push('/')
            }
        }
    }
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

    #nav {
        padding: 30px;

    }
    .router-link-exact-active {
        color: #42b983;
    }
    a {
        font-weight: bold;
        color: #42b983;
    }
</style>
