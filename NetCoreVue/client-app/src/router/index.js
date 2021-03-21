import Vue from 'vue'
import VueRouter from 'vue-router'

import Auth from '@okta/okta-vue'

Vue.use(Auth, {
    issuer: 'https://dev-92155099.okta.com/oauth2/default',
    client_id: '0oacdi7pgNbBoHgPx5d6',
    redirect_uri: 'https://localhost:44346/implicit/callback',
    scope: 'openid profile email'
})

Vue.use(VueRouter)

const routes = [
    {
        path: '/',
        name: 'Home',
        component: () => import(/* webpackChunkName: "about" */ '../views/Home.vue')
    },
    {
        path: '/teams',
        name: 'Teams',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "about" */ '../views/Teams.vue'),
        meta: {
            requiresAuth: true
        }
    },
    {
        path: '/add',
        name: 'Add',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "about" */ '../components/Add.vue'),
        meta: {
            requiresAuth: true
        }
    },
    {
        path: '/edit/:id',
        name: 'Edit',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "about" */ '../components/Edit.vue'),
        meta: {
            requiresAuth: true
        }
    },
    {
        path: '/implicit/callback',
        component: Auth.handleCallback()
    }
]

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
})

router.beforeEach(Vue.prototype.$auth.authRedirectGuard())

export default router