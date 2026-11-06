import { useState } from "react";
import { Review } from "../_types/review";
import { stimeRequest } from "../stime-interceptor";

const domain = "https://localhost:7013/";

export function useStimeAPI() {

    const [reviews, setReviews] = useState<Review[]>([]);

    async function getReviews() {

        const x = await stimeRequest.get(domain + "api/Reviews/GetReview");
        console.log(x.data);
        setReviews(x.data);

    }

    async function postReview(text: string, game: string) {

        const reviewDTO = {
            text: text,
            game: game
        };

        const x = await stimeRequest.post(domain + "api/Reviews/PostReview", reviewDTO);
        console.log(x.data);
        
        getReviews();

    }

    async function deleteReview(id: number) {

        const x = await stimeRequest.delete(domain + "api/Reviews/DeleteReview/" + id);
        console.log(x.data);

        getReviews();

    }

    async function editReview(id: number, text: string) {

        const reviewDTO = {
            text: text,
            game: ""
        };

        const x = await stimeRequest.put(domain + "api/Reviews/EditReview/" + id, reviewDTO);
        console.log(x.data);

        getReviews();

    }

    async function upvoteReview(id: number) {

        let x = await stimeRequest.put(domain + "api/Reviews/UpvoteReview/" + id);
        console.log(x.data);

        getReviews();

    }

    return { 

        // États
        reviews, 

        // Reviews
        getReviews, postReview, upvoteReview, deleteReview, editReview
    }

}