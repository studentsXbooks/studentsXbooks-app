import React from "react";
import { Link } from "@reach/router";
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography
} from "@material-ui/core";

type Props = {
  listing: {
    title: string,
    price: string,
    id: string,
    isbn10: String,
    condition: string,
    authors: [string]
  }
};

const ListingCard = ({
  listing: { title, price, id, isbn10, condition, authors }
}: Props) => (
  <Grid item>
    {/* //$FlowFixMe */}
    <Link to={`/listing/${id}`}>
      <Card raised>
        <CardHeader
          title={
            <>
              <Typography variant="h5">{title}</Typography>
              <Typography variant="h6">By: {authors.join(",")}</Typography>
            </>
          }
        />
        <CardContent>
          <Typography>{condition}</Typography>
          <Typography>{isbn10}</Typography>
          <Typography>${price}</Typography>
        </CardContent>
      </Card>
    </Link>
  </Grid>
);

export default ListingCard;
