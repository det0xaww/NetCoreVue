<template>
    <div class="add">
        <h1>Add NBA team</h1>
        <div id="divToEdit" class="d-flex justify-content-center ml-5 mr-5 pl-5 pr-5">
            <form method="post" @submit.prevent="createNbaTeam">
                <div class="row">
                    <div class="form-group col-lg-12 col-sm-12">
                        <p class="border border-danger" v-if="errors.errors.length">
                            <ul>
                                <li class="text-danger" v-for="error in errors.errors" :key="error.key">{{ error }}</li>
                            </ul>
                        </p>
                    </div>
                    <div class="form-group col-lg-6 col-sm-12">
                        <label>Abbreviation</label>
                        <input class="form-control" v-model="model.abbreviation" type="text" />
                    </div>
                    <div class="form-group col-lg-6 col-sm-12">
                        <label>City</label>
                        <input class="form-control" v-model="model.city" type="text" />
                    </div>
                    <div class="form-group col-lg-6 col-sm-12">
                        <label>Conference</label>
                        <input class="form-control" v-model="model.conference" type="text" />
                    </div>
                    <div class="form-group col-lg-6 col-sm-12">
                        <label>Division</label>
                        <input class="form-control" v-model="model.division" type="text" />
                    </div>
                    <div class="form-group col-lg-6 col-sm-12">
                        <label>Fullname</label>
                        <input class="form-control " v-model="model.full_name" type="text" />
                    </div>
                    <div class="form-group col-lg-6 col-sm-12">
                        <label>Name</label>
                        <input class="form-control" v-model="model.name" type="text" />
                    </div>
                    <div class="form-group col-lg-12 col-sm-12">
                        <button @click.prevent="$router.back()" class="btn btn-primary mr-3">Cancel</button>
                        <input id="submitBtn" type="submit" variant="success" class="btn btn-success" value="Add" />
                    </div>
                </div>
</form>
        </div>
    </div>
</template>
<script>
    import api from '@/NBATeamsApiService'

    export default {
        data() {
            return {
                model: {},
                errors: { key:1,errors:[]}
            };
        },
        methods: {
            async createNbaTeam() {
                if (!this.model.abbreviation) {
                    this.errors.errors.push('Abbreviation is required.');
                    this.key++;
                }
                if (!this.model.city) {
                    this.errors.errors.push('City is required.');
                    this.key++;
                }
                if (!this.model.conference) {
                    this.errors.errors.push('Conference is required.');
                    this.key++;
                }
                if (!this.model.division) {
                    this.errors.errors.push('Division is required.');
                    this.key++;
                }
                if (!this.model.full_name) {
                    this.errors.errors.push('Fullname is required.');
                    this.key++;
                }
                if (!this.model.name) {
                    this.errors.errors.push('Name is required.');
                    this.key++;
                }

                await api.create(this.model)

                // Clear the data inside of the form
                this.model = {}

                // Fetch all records again to have latest data
                await api.getAll("")

                this.errors.errors.length = 0; //to reset errors messages
            }
        }
    }
</script>
